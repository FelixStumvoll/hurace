import React from 'react';
import styled from 'styled-components';
import { DisciplineData } from '../../../models/DisciplineData';
import { RaceListViewItem } from '../../shared/race/RaceListViewItem';
import { DefaultLink, FlexWrap } from '../../../theme/CustomComponents';
import { HeaderCard } from '../../shared/HeaderCard';

const PanelWrapper = styled.div`
    margin-bottom: ${props => props.theme.gap};
`;

export const DisciplineViewItem: React.FC<{
    disciplineData: DisciplineData;
    seasonId: number;
}> = ({ disciplineData, seasonId }) => {
    return (
        <PanelWrapper>
            <HeaderCard headerText={disciplineData.discipline.disciplineName}>
                <FlexWrap>
                    {disciplineData.races.map(r => (
                        <DefaultLink
                            key={r.id}
                            to={`/season/${seasonId}/race/${r.id}`}
                        >
                            <RaceListViewItem race={r} />
                        </DefaultLink>
                    ))}
                </FlexWrap>
            </HeaderCard>
        </PanelWrapper>
    );
};
