import React from 'react';
import { Season } from '../../interfaces/Season';
import { Card } from '../../theme/StyledComponents';
import styled from 'styled-components';
import { useSelector, useDispatch } from 'react-redux';
import { StoreState } from '../../store/rootReducer';
import {
    expandSeason,
    collapseSeason
} from '../../store/reducers/season-expander-reducer/seasonExpanderActions';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faAngleDown, faAngleUp } from '@fortawesome/free-solid-svg-icons';
import { DisciplineView } from './discipline-view/DisciplineView';

const SeasonBanner = styled.div`
    width: 100%;
    display: flex;
    cursor: pointer;
`;

const ExpanderText = styled.div`
    margin-left: auto;
    color: gray;
    height: 100%;
`;

const SeasonItem = styled(Card)`
    margin-bottom: 10px;
    display: flex;
    flex-direction: column;
`;

export const SeasonViewItem: React.FC<{ season: Season }> = ({ season }) => {
    const expanded = useSelector(
        (state: StoreState) => state.seasonExpander.seasonExpanded === season.id
    );

    const dispatch = useDispatch();
    return (
        <SeasonItem>
            <SeasonBanner
                onClick={() =>
                    dispatch(
                        expanded ? collapseSeason() : expandSeason(season.id)
                    )
                }
            >
                <div>
                    {season.startDate.getFullYear()} <b>-</b>{' '}
                    {season.endDate.getFullYear()}
                </div>
                <ExpanderText>
                    <FontAwesomeIcon
                        icon={expanded ? faAngleUp : faAngleDown}
                    />
                </ExpanderText>
            </SeasonBanner>

            {expanded && <DisciplineView seasonId={season.id} />}
        </SeasonItem>
    );
};
