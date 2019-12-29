import React, { useState } from 'react';
import { Season } from '../../interfaces/Season';
import { ItemCard, Card } from '../../theme/StyledComponents';
import { Link } from 'react-router-dom';
import styled from 'styled-components';
import { DisciplineViewItem } from '../discipline-view/DisciplineViewItem';
import { useSelector, useDispatch } from 'react-redux';
import { StoreState } from '../../store/rootReducer';
import {
    expandSeason,
    collapseSeason
} from '../../store/reducers/seasonExpanderReducer/seasonExpanderActions';

const SeasonBanner = styled.div`
    width: 100%;
    display: flex;
`;

const ExpanderText = styled.div`
    margin-left: auto;
    color: gray;
`;

const SeasonItem = styled(Card)`
    margin-bottom: 10px;
    cursor: pointer;
    padding: 25px;
    display: flex;
    flex-direction: column;
`;

const SeasonDetails = styled.div`
    margin-top: 20px;
    max-height: 500px;
    width: 100%;
    overflow: auto;
`;

export const SeasonViewItem: React.FC<{
    season: Season;
}> = ({ season }) => {
    const disciplines = useSelector(
        (state: StoreState) => state.disciplines.disciplines
    );
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
                    {expanded ? 'Einklappen' : 'Ausklappen'}
                </ExpanderText>
            </SeasonBanner>

            {expanded && (
                <SeasonDetails>
                    {disciplines.map(d => (
                        <DisciplineViewItem key={d.id} discipline={d} />
                    ))}
                </SeasonDetails>
            )}
        </SeasonItem>
    );
};
